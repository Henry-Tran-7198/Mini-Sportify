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
    if (a = 0){
        printf("This is an linear equation. \n");
    }
    else {
        printf("Quadratic equation. \n");
    }
    int Delta;
    Delta = b*b - 4*a*c;
    printf("Delta = b*b - 4*a*c \n");
    if (Delta > 0){
        printf("Equation has 2 different solutions. \n");
    }
    else if (Delta = 0){
        printf("Equation has repeated roots. \n");
    }
    else {
        printf("Equation has no solution. \n");
    }
}