void setup() {
  Serial.begin(9600);
  pinMode(5, OUTPUT); // Красный
  pinMode(6, OUTPUT); // Зеленый
  pinMode(9, OUTPUT); // Синий
}

int i = 0;

void loop() {}

void serialEvent() {
  while(Serial.available()){ // Возвращает количество байт в буфере доступных для считывания
    byte val = Serial.read();
    if(i == 0) analogWrite(5, val);
    else if (i == 1) analogWrite(6, val);
    else if(i == 2) analogWrite(9, val);
    ++i;
    if (i == 3) i = 0;
  }
}
