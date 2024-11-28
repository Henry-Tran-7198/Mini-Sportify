#include <stdio.h>
int main (){
    int a;
    int b;
    int c;
    int d;
    printf("Input a: ");
    scanf("%d", &a);
    printf("Input b: ");
    scanf("%d", &b);
    printf("Input c: ");
    scanf("%d", &c);
    printf("Input d: ");
    scanf("%d", &d);
    if (a > b && a > c && a > d){
        printf("Max of 4 numbers is: %d", a);
    }
    else if (b > c && b > d && b > a){
        printf("Max of 4 numbers is: %d", b);
    }
    else if (c > d && c > a && c > b){
        printf("Max of 4 numbers is: %d", c);
    }
    else{
        printf("Max of 4 numbers is: %d", d);
    }
}