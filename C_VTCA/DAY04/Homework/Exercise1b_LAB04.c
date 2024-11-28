#include <stdio.h>
int main (){
    int a;
    int b;
    int c;
    printf("Input a: ");
    scanf("%d", &a);
    printf("Input b: ");
    scanf("%d", &b);
    printf("Input c: ");
    scanf("%d", &c);
    if (a > b && a > c){
        printf("Max of a, b, c is: %d \n", a);
    }
    else if (b > a && b > c){
        printf("Max of a, b, c is: %d \n", b);
    }
    else {
        printf("Max of a, b, c is: %d \n", c);
    }
}