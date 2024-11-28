#include <stdio.h>
int main(){
    int i;
    int j;
    int n;
    printf("Input the number of elements in prime number list: ");
    scanf("%d", &n);
    printf("Prime numbers list:");
    for (i = 2; i < n; i++){
        for (j = 2; j <= i; j++)
            if (i % j == 0) break;
            if (j > (i/j)) printf(" %d", i);
        
    }
}