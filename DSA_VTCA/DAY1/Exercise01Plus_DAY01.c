#include <stdio.h>
int F(int n){
    if (n <= 1){
        return n;
    }
    else {
        return F(n-1) + F(n-2);
    }
}
int main(int argc, char const *argv[])
{
    int n;
    printf("Input value n: ");
    scanf("%d", &n);
    for (int i = 0; i < n; i++){
        printf("%d ", F(i));
    }
    return 0;
}
