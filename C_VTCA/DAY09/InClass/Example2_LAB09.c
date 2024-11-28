#include <stdio.h>
void swap(int x, int y){
    int temp;
    temp = x;
    x = y;
    y = temp;
    printf("Value a after swap: %d \n", x);
    printf("Value b after swap: %d \n", y);
    return;
}
int main(){
    int a;
    int b;
    printf("Input a: ");
    scanf("%d", &a);
    printf("Input b: ");
    scanf("%d", &b);
    printf("Value a be4 swap: %d \n", a);
    printf("Value b be4 swap: %d \n", b);
    swap(a, b);
}