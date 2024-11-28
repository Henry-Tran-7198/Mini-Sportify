#include <stdio.h>
int main(){
    int num1;
    int num2;
    printf("Input num 1: ");
    scanf("%d", &num1);
    printf("Input num 2: ");
    scanf("%d", &num2);
    if (num1 > num2){
        printf("Num 1 is the biggest \n");
    }
    else{
        printf("Num 2 is the biggest \n");
    }
}