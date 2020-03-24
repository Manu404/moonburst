using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
// ReSharper disable All

namespace MoonBurst.Core.Hardware
{
    [Serializable]
    public class TeVirtualMidiException : Exception
    {
        /* defines of specific WIN32-error-codes that the native teVirtualMIDI-driver
		 * is using to communicate specific problems to the application */
        private const int ErrorPathNotFound = 3;
        private const int ErrorInvalidHandle = 6;
        private const int ErrorTooManyCmds = 56;
        private const int ErrorTooManySess = 69;
        private const int ErrorInvalidName = 123;
        private const int ErrorModNotFound = 126;
        private const int ErrorBadArguments = 160;
        private const int ErrorAlreadyExists = 183;
        private const int ErrorOldWinVersion = 1150;
        private const int ErrorRevisionMismatch = 1306;
        private const int ErrorAliasExists = 1379;

        public TeVirtualMidiException()
        {
        }

        public TeVirtualMidiException(string message) : base(message)
        {
        }

        public TeVirtualMidiException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TeVirtualMidiException(SerializationInfo info, StreamingContext context)
        {
        }

        private int _reasonCode;
        public int ReasonCode
        {
            get => _reasonCode;
            set => _reasonCode = value;
        }


        private static string ReasonCodeToString(int reasonCode)
        {
            switch (reasonCode)
            {
                case ErrorOldWinVersion:
                    return "Your Windows-version is too old for dynamic MIDI-port creation.";
                case ErrorInvalidName:
                    return "You need to specify at least 1 character as MIDI-portname!";
                case ErrorAlreadyExists:
                    return "The name for the MIDI-port you specified is already in use!";
                case ErrorAliasExists:
                    return "The name for the MIDI-port you specified is already in use!";
                case ErrorPathNotFound:
                    return "Possibly the teVirtualMIDI-driver has not been installed!";
                case ErrorModNotFound:
                    return "The teVirtualMIDIxx.dll could not be loaded!";
                case ErrorRevisionMismatch:
                    return "The teVirtualMIDIxx.dll and teVirtualMIDI.sys driver differ in version!";
                case ErrorTooManySess:
                    return "Maximum number of ports reached";
                case ErrorInvalidHandle:
                    return "Port not enabled";
                case ErrorTooManyCmds:
                    return "MIDI-command too large";
                case ErrorBadArguments:
                    return "Invalid flags specified";
                default:
                    return "Unspecified virtualMIDI-error: " + reasonCode;
            }
        }


        public static void ThrowExceptionForReasonCode(int reasonCode)
        {

            TeVirtualMidiException exception = new TeVirtualMidiException(ReasonCodeToString(reasonCode));

            exception.ReasonCode = reasonCode;

            throw exception;

        }

    }

    public class TeVirtualMidi
    {

        /* default size of sysex-buffer */
        private const UInt32 TeVmDefaultSysexSize = 65535;

        /* constant for loading of teVirtualMIDI-interface-DLL, either 32 or 64 bit */
        private const string DllName = "teVirtualMIDI64.dll";
        /* private const string DllName = "teVirtualMIDI32.dll"; */
        /* private const string DllName = "teVirtualMIDI64.dll"; */

        /* TE_VM_LOGGING_MISC - log internal stuff (port enable, disable...) */
        public const UInt32 TeVmLoggingMisc = 1;
        /* TE_VM_LOGGING_RX - log data received from the driver */
        public const UInt32 TeVmLoggingRx = 2;
        /* TE_VM_LOGGING_TX - log data sent to the driver */
        public const UInt32 TeVmLoggingTx = 4;

        /* TE_VM_FLAGS_PARSE_RX - parse incoming data into single, valid MIDI-commands */
        public const UInt32 TeVmFlagsParseRx = 1;
        /* TE_VM_FLAGS_PARSE_TX - parse outgoing data into single, valid MIDI-commands */
        public const UInt32 TeVmFlagsParseTx = 2;
        /* TE_VM_FLAGS_INSTANTIATE_RX_ONLY - Only the "midi-out" part of the port is created */
        public const UInt32 TeVmFlagsInstantiateRxOnly = 4;
        /* TE_VM_FLAGS_INSTANTIATE_TX_ONLY - Only the "midi-in" part of the port is created */
        public const UInt32 TeVmFlagsInstantiateTxOnly = 8;
        /* TE_VM_FLAGS_INSTANTIATE_BOTH - a bidirectional port is created */
        public const UInt32 TeVmFlagsInstantiateBoth = 12;

        /* static initializer to retrieve version-info from DLL... */
        static TeVirtualMidi()
        {
            _versionString = Marshal.PtrToStringAuto(virtualMIDIGetVersion(ref _versionMajor, ref _versionMinor, ref _versionRelease, ref _versionBuild));
            _driverVersionString = Marshal.PtrToStringAuto(virtualMIDIGetDriverVersion(ref _driverVersionMajor, ref _driverVersionMinor, ref _driverVersionRelease, ref _driverVersionBuild));
        }

        public TeVirtualMidi(string portName, UInt32 maxSysexLength = TeVmDefaultSysexSize, UInt32 flags = TeVmFlagsParseRx)
        {
            _instance = virtualMIDICreatePortEx2(portName, IntPtr.Zero, IntPtr.Zero, maxSysexLength, flags);

            if (_instance == IntPtr.Zero)
            {
                int lastError = Marshal.GetLastWin32Error();
                TeVirtualMidiException.ThrowExceptionForReasonCode(lastError);
            }

            _readBuffer = new byte[maxSysexLength];
            _readProcessIds = new UInt64[17];
            _maxSysexLength = maxSysexLength;
        }

        public TeVirtualMidi(string portName, UInt32 maxSysexLength, UInt32 flags, ref Guid manufacturer, ref Guid product)
        {
            _instance = virtualMIDICreatePortEx3(portName, IntPtr.Zero, IntPtr.Zero, maxSysexLength, flags, ref manufacturer, ref product);

            if (_instance == IntPtr.Zero)
            {
                int lastError = Marshal.GetLastWin32Error();
                TeVirtualMidiException.ThrowExceptionForReasonCode(lastError);
            }

            _readBuffer = new byte[maxSysexLength];
            _readProcessIds = new UInt64[17];
            _maxSysexLength = maxSysexLength;
        }

        ~TeVirtualMidi()
        {
            if (_instance != IntPtr.Zero)
            {
                virtualMIDIClosePort(_instance);
                _instance = IntPtr.Zero;
            }
        }



        public static UInt32 Logging(UInt32 loggingMask)
        {
            return virtualMIDILogging(loggingMask);
        }

        public void Shutdown()
        {
            if (!virtualMIDIShutdown(_instance))
            {
                int lastError = Marshal.GetLastWin32Error();
                TeVirtualMidiException.ThrowExceptionForReasonCode(lastError);
            }
        }

        public void SendCommand(byte[] command)
        {
            if ((command == null) || (command.Length == 0))
            {
                return;
            }

            if (!virtualMIDISendData(_instance, command, (UInt32)command.Length))
            {
                int lastError = Marshal.GetLastWin32Error();
                TeVirtualMidiException.ThrowExceptionForReasonCode(lastError);
            }
        }

        public byte[] GetCommand()
        {
            UInt32 length = _maxSysexLength;

            if (!virtualMIDIGetData(_instance, _readBuffer, ref length))
            {
                int lastError = Marshal.GetLastWin32Error();
                TeVirtualMidiException.ThrowExceptionForReasonCode(lastError);
            }

            byte[] outBytes = new byte[length];
            Array.Copy(_readBuffer, outBytes, length);
            return outBytes;
        }
        
        public UInt64[] GetProcessIds()
        {

            UInt32 length = 17 * sizeof(ulong);
            UInt32 count;

            if (!virtualMIDIGetProcesses(_instance, _readProcessIds, ref length))
            {
                int lastError = Marshal.GetLastWin32Error();
                TeVirtualMidiException.ThrowExceptionForReasonCode(lastError);
            }

            count = length / sizeof(ulong);
            UInt64[] outIds = new UInt64[count];
            Array.Copy(_readProcessIds, outIds, count);
            return outIds;
        }

        public static int VersionMajor => _versionMajor;
        public static int VersionMinor => _versionMinor;
        public static int VersionRelease => _versionRelease;
        public static int VersionBuild => _versionBuild;
        public static String VersionString => _versionString;
        public static int DriverVersionMajor => _driverVersionMajor;
        public static int DriverVersionMinor => _driverVersionMinor;
        public static int DriverVersionRelease => _driverVersionRelease;
        public static int DriverVersionBuild => _driverVersionBuild;
        public static String DriverVersionString => _driverVersionString;
        
        private readonly byte[] _readBuffer;
        private IntPtr _instance;
        private readonly UInt32 _maxSysexLength;
        private readonly UInt64[] _readProcessIds;
        private static readonly ushort _versionMajor;
        private static readonly ushort _versionMinor;
        private static readonly ushort _versionRelease;
        private static readonly ushort _versionBuild;
        private static readonly String _versionString;

        private static readonly ushort _driverVersionMajor;
        private static readonly ushort _driverVersionMinor;
        private static readonly ushort _driverVersionRelease;
        private static readonly ushort _driverVersionBuild;
        private static readonly String _driverVersionString;
        
        [DllImport(DllName, EntryPoint = "virtualMIDICreatePortEx3", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr virtualMIDICreatePortEx3(string portName, IntPtr callback, IntPtr dwCallbackInstance, UInt32 maxSysexLength, UInt32 flags, ref Guid manufacturer, ref Guid product);

        [DllImport(DllName, EntryPoint = "virtualMIDICreatePortEx2", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr virtualMIDICreatePortEx2(string portName, IntPtr callback, IntPtr dwCallbackInstance, UInt32 maxSysexLength, UInt32 flags);

        [DllImport(DllName, EntryPoint = "virtualMIDIClosePort", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern void virtualMIDIClosePort(IntPtr instance);

        [DllImport(DllName, EntryPoint = "virtualMIDIShutdown", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern Boolean virtualMIDIShutdown(IntPtr instance);

        [DllImport(DllName, EntryPoint = "virtualMIDISendData", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern Boolean virtualMIDISendData(IntPtr midiPort, byte[] midiDataBytes, UInt32 length);

        [DllImport(DllName, EntryPoint = "virtualMIDIGetData", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern Boolean virtualMIDIGetData(IntPtr midiPort, [Out] byte[] midiDataBytes, ref UInt32 length);

        [DllImport(DllName, EntryPoint = "virtualMIDIGetProcesses", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern Boolean virtualMIDIGetProcesses(IntPtr midiPort, [Out] UInt64[] processIds, ref UInt32 length);

        [DllImport(DllName, EntryPoint = "virtualMIDIGetVersion", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr virtualMIDIGetVersion(ref ushort major, ref ushort minor, ref ushort release, ref ushort build);

        [DllImport(DllName, EntryPoint = "virtualMIDIGetDriverVersion", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr virtualMIDIGetDriverVersion(ref ushort major, ref ushort minor, ref ushort release, ref ushort build);

        [DllImport(DllName, EntryPoint = "virtualMIDILogging", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern UInt32 virtualMIDILogging(UInt32 loggingMask);
    }
}
