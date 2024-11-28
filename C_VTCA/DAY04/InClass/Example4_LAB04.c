#include <stdio.h>
int main(){
    int num;
    printf("Accept a integer number");
    scanf("%d", &num);
    int r = num % 2;
    if (r==0){
        printf("Number is Even. \n");
    }
    else{
        printf("Number is Odd. \n");
    }
}