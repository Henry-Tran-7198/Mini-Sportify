#include <stdio.h>
int sum(int a, int b){
        return a + b;
    };
int main(){
    int num1;
    int num2;
    printf("Input your num 1: ");
    scanf("%d", &num1);
    printf("Input your num 2: ");
    scanf("%d", &num2);
    printf("Your sum of %d and %d is: %d", num1, num2, sum(num1, num2));
}