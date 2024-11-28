#include <stdio.h>
int main(){
    int a, b;
    printf("Input a: ");
    scanf("%d", &a);
    printf("Input b: ");
    scanf("%d", &b);
    int* p = &a;
    int* q = &b;
    printf("%p \n", p);
    printf("%d \n", *p);
    printf("%p \n", q);
    printf("%d \n", *q);
    printf("Change value of a and b: \n");
    int NEWa = b;
    int NEWb = a;
    int* newa = &NEWa;
    int* newb = &NEWb;
    printf("%p \n", newa);
    printf("%d \n", *newa);
    printf("%p \n", newb);
    printf("%d \n", *newb);
}