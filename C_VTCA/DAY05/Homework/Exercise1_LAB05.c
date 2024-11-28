#include <stdio.h>
int main(){
    int A;
    int B;
    printf("Input A: ");
    scanf("%d", &A);
    printf("Input B: ");
    scanf("%d", &B);
    float X;
    X = (float) -B / A;
    if (A == 0){
        if (B == 0){
            printf("The equation is true for any number X");
        }
        else{
            printf("The equation has no root.");
        }
    }
    else{
        printf("X = - %d / %d = %0.3f", B, A, X);
    }
}