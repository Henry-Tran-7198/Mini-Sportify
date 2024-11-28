#include <stdio.h>
int main(){
    int a;
    scanf("Input a: %d \n", &a);
    int b;
    scanf("Input b: %d \n", &b);
    printf("Your sum is:  %d \n", a + b);
    printf("Your difference is: %d \n", a - b);
    printf("Your product is: %d \n", a * b);
    printf("Your quotient is: %d \n", a % b);
}