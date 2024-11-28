#include <stdio.h>
#include "max.c"
int main(){
    int a, b, ret;
    printf("Value a: ");
    scanf("%d", &a);
    printf("Value b: ");
    scanf("%d", &b);
    ret = max(a, b);
    printf("Max value is: %d \n", ret);
    return 0;
}