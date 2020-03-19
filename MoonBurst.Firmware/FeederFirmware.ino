const int digitalPortCount = 6;
const int analogPortCount = 2;
const int serialSpeed = 9600;

void initPort(int padding, int id) {
	pinMode(padding + (id * 2), INPUT_PULLUP);
	pinMode(padding + ((id * 2) + 1), INPUT_PULLUP);
}

void setup() {
	Serial.begin(serialSpeed);
	for (int i = 0; i <= digitalPortCount; initPort(2, i++));
	for (int i = 0; i <= analogPortCount; initPort(A0, i++));
}

void loop() {
	// send pin registers over serial in human readble format for debug/trouble
	// format: ?[D0 - D7];[D7 -  D13];[A0 - A5]!
	Serial.println("?" + String(PIND, DEC) + ";" + String(PINB, DEC) + ";" + String(PINC, DEC) + "!");
}
