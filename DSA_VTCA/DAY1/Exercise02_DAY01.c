#include <stdio.h>
int main(){
    int num1;
    int num2;
    printf("Input num 1: ");
    scanf("%d", &num1);
    printf("Input num 2: ");
    scanf("%d", &num2);
    int sum = 0;
    sum = num1 + num2;
    printf("Sum of %d and %d: %d \n", num1, num2, sum);
}