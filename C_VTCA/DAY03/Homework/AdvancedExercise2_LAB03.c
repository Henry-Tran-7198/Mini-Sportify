#include <stdio.h>
int main(){
   int num1;
   int num2;
    printf("Input number 1: ");
    scanf("%d", &num1);
    printf("Input number 2: ");
    scanf("%d", &num2);
    printf("Result of AND: %d \n", num1 & num2);
    printf("Result of OR: %d \n", num1 | num2);
    printf("Result of XOR: %d \n", num1 ^ num2);
    printf("Result of NOT for number 1: %d \n", ~ num1);
}