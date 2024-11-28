#include <stdio.h>
int main(){
    int a;
    int b;
    printf("Input first number: ");
    scanf("%d", &a);
    printf("Input second number: ");
    scanf("%d", &b);
    int sum;
    sum = a + b;
    int difference;
    difference = a - b;
    int product;
    product = a * b;
    float quotient;
    quotient = (float) a / b;
    printf("       MENU     \n");
    printf("================== \n");
    printf("1. +\n");
    printf("2. -\n");
    printf("3. : \n");
    printf("4. x \n");
    printf("================== \n");
    int choice;
    printf("Choose: ");
    scanf("%d", &choice);
    if (choice < 1 || choice > 4){
        printf("No calculation \n");
    }
    else {
        switch(choice){
            case 1:
            printf("Sum: %d + %d = %d", a, b, sum);
            break;
            case 2:
            printf("Difference: %d - %d = %d", a, b, difference);
            break;
            case 3:
            printf("Quotient: %d : %d = %f", a, b, product);
            break;
            default:
            printf("Product: %d x %d = %d", a, b, quotient);
            break;
        }
    }
}