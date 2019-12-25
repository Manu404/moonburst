const int firstPin = 2;
const int pinCount = 3;
const int lastPin = firstPin + pinCount;
String state = "";

void setup() {
	Serial.begin(9600);
	for (int i = firstPin; i < lastPin; i++)
		pinMode(i, INPUT);

	pinMode(12, INPUT_PULLUP);
	pinMode(13, INPUT_PULLUP);
}

void loop() {
	state = "";
	for (int i = firstPin; i < lastPin; i++)
		state += String(digitalRead(i), DEC);
	//Serial.println(state);

	if (digitalRead(12) == LOW && digitalRead(13) == LOW) {
		Serial.println("3");
	}

	else if (digitalRead(13) == LOW) {
		Serial.println("2");
	}

	else if (digitalRead(12) == LOW) {
		Serial.println("1"); 
	}
}