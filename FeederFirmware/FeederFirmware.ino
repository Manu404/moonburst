const int firstPin = 2;
const int pinCount = 3;
const int lastPin = firstPin + pinCount;
String state = "";

void setup() {
	Serial.begin(9600);
	pinMode(12, INPUT_PULLUP);
	pinMode(13, INPUT_PULLUP);
}

void loop() {
	state = "";


	if (digitalRead(12) == LOW && digitalRead(13) == LOW) {
		state += "001";
	}
	else {
		if (digitalRead(13) == LOW) {
			state += "1";
		}
		else {
			state += "0";
		}

		if (digitalRead(12) == LOW) {
			state += "1";
		}
		else {
			state += "0";
		}
		state += "0";
	}

	Serial.println(state);
}