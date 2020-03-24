﻿using System.Collections.Generic;

namespace MoonBurst.Core.Helper
{
    public interface IDynamicsHelper
    {
        List<Dynamic> AvailableDynamics { get; }
    }

    public class DynamicsHelper : IDynamicsHelper
    {
        public List<Dynamic> AvailableDynamics { get; }

        public DynamicsHelper()
        {
            int[] midi = { 10, 23, 36, 49, 62, 75, 88, 101, 114, 127 };
            string[] name = { "pppp", "ppp", "pp", "p", "mp", "mf", "f", "ff", "fff", "ffff" };
            string[] display = { "OO", "OP", "O", "P", "mP", "mF", "F", "D", "DF", "DD" };

            AvailableDynamics = new List<Dynamic>();

            for(var i = 0; i < midi.Length; i++)
            {
                AvailableDynamics.Add(new Dynamic(name[i],midi[i],display[i]));
            }
        }
    }

}
