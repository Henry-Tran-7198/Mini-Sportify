#include <stdio.h>
int main(){
    printf("1 meter = 0.000621371192 mile \n");
    printf("====================== \n");
    printf("Please input your kilometers: ");
    float YourKmNumber;
    scanf("%f", &YourKmNumber);
    float YourMileNumber;
    YourMileNumber = (float) YourKmNumber * 0.621371192;
    printf("Result: \n");
    printf("%f km = %0.7f mile", YourKmNumber, YourMileNumber);
}