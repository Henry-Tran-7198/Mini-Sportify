#include <stdio.h>
int main(){
    int a = 5;
    while (a < 100){
        printf("Value of a: %d \n", a);
        a++;
    }
    int i;
    for (i = 10; i < 50; i = i + 1){
        if (i == 15){
            continue;
        }
        printf("Value of i: %d \n", i);
    }
    int b = 10;
    do {
        printf("Value of b: %d \n", b);
        b = b + 1;
    }
    while (b < 20);
    int c, d;
    for (c = 2; c < 100; c++){
        for (d = 2; d <= (c/d); d++)
            if(!(c % d)) break;
            if (d > (c / d)) printf("%d is prime \n", c);
    }
    int e;
    for (e = 10; e < 20; e = e + 1){
        if (e == 13){
            break;
        }
        printf("Value of e: %d \n", e);
    }
    int f;
    for (f = 10; f < 20; f = f + 1){
        if (f == 13){
            continue;
        }
        printf("Value of f: %d \n", f);
    }
}