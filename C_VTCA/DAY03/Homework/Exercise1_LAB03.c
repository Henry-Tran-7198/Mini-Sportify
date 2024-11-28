#include <stdio.h>
int main() {
int num1;
scanf("%d", &num1);
int num2;
scanf("%d", &num2);
printf("Number 1 = %d \n", num1);
printf("Number 2 = %d \n", num2);
int newnum1;
newnum1 = num2;
printf("New value of number 1: %d \n", ++newnum1);
int newnum2;
newnum2 = num1;
printf("New value of number 2: %d \n", ++newnum2);
return 0;
}