#include <stdio.h>
int main(){
    int a[5] = {1, 3, 5, 7, 9};
    int b[5] = {2, 4, 6, 8, 10};
    int c[5];
    printf("Value of Array c is: ");
    for (int i = 0; i < 5; i++){
        printf("%d ",  c[i] = a[i] + b[i]);
    }
}