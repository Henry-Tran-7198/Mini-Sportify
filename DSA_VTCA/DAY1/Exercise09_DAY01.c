#include <stdio.h>
int main(){
    int num;
    int i;
    printf("Input your number: ");
    scanf("%d", &num);
    while (num < 0){
        printf("Invalid number. Input again. \n");
        printf("Input your number: ");
        scanf("%d", &num);
    }
    int factorial = 1;
    for (i = num; i > 0; i--){
        factorial = factorial * i;
    }
    printf("Factorial of %d is: %d", num, factorial);
}