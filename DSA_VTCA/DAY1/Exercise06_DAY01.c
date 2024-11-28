#include <stdio.h>
int main(){
    int num;
    int absValue;
    printf("Input a int num: ");
    scanf("%d", &num);
    if (num < 0){
        absValue = -num;
    }
    else{
        absValue = num;
    }
    printf("Absolute value of %d is: %d ", num, absValue);
}