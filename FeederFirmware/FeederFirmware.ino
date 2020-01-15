const int portCount = 4;
const int pinPadding = 0;
const int serialSpeed = 9600;

String state = "";

void initPort(int id) 
{
	pinMode(id * 2, INPUT_PULLUP);
	pinMode((id * 2) + 1, INPUT_PULLUP);
}

void readPort(int id)
{
	int ring = (id * 2) + pinPadding;
	int tip = ring + 1;
	state += " ";

	if (digitalRead(ring) == LOW && digitalRead(tip) == LOW) {
		state += "001";
	}
	else {
		if (digitalRead(tip) == LOW) {
			state += "1";
		}
		else {
			state += "0";
		}

		if (digitalRead(ring) == LOW) {
			state += "1";
		}
		else {
			state += "0";
		}
		state += "0";
	}
}

void setup() {
	Serial.begin(serialSpeed);
	for (int i = 1; i <= portCount; initPort(i++));
}

void loop() {
	state = "";
	for (int i = 1; i <= portCount; readPort(i++));
	Serial.println(state);
}