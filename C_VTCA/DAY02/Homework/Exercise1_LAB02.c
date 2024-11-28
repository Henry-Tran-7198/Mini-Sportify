#include <stdio.h>
int main() {
    int myInt;
    float myFloat;
    double myDouble;
    char myChar;
    printf("Byte number of integer data type is: %lu \n", sizeof(myInt));
    printf("Byte number of decimal data type is: %lu \n", sizeof(myFloat));
    printf("Byte number of float data type is: %lu \n", sizeof(myDouble));
    printf("Byte number of character data type is: %lu \n", sizeof(myChar));
}
