#include <stdio.h>
int main()
{
    int m;
    int n;
    printf("Input m: ");
    scanf("%d", &m);
    printf("Input n: ");
    scanf("%d", &n);
    int i;
    i = m;
    for (m = m; m < n; m++){
        if (m % 7 != 0) continue;
        if (m % 7 == 0) printf("%d can divide with 7 from %d to %d \n", m, i, n);
    }
    return 0;
}