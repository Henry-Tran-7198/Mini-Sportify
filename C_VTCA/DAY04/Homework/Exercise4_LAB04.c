#include <stdio.h>
int main(){
    int a;
    int b;
    int c;
    printf("Value of a: ");
    scanf("%d", &a);
    printf("Value of b: ");
    scanf("%d", &b);
    printf("Value of c: ");
    scanf("%d", &c);
    if (a + b > c && a + c > b){
        printf("Three side of triangle can create a triangle. \n");
    }
    else{
        printf("Three side of triangle can't create a triangle. ");
    }
}