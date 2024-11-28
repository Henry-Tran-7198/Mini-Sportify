#include <stdio.h>
int main (){
    int a;
    int b;
    printf("Input a: ");
    scanf("%d", &a);
    printf("Input b: ");
    scanf("%d", &b);
    float x = (float) -b / a;
    if (a = 0){
        if (b = 0){
            printf("Equation has infinity solutions \n");
        } else {
            printf("Equation has no solution \n");
        }
    } else {
        printf("x = %0.3f \n", x);
    }
}