#include <stdio.h>
#include <stdlib.h>
int main(){
    int *a;
    int count, i;
    printf("Input array size: ");
    scanf("%d", &count);
    a = (int*)malloc(count*sizeof(int));
    for (i = 0; i < count; i++){
        printf("a[%d] = ", i);
        scanf("%d", a + i);
    }
    printf("\nArray: \n");
    for (i = 0; i < count; i++){
        printf("%d", a[i]);
    }
    free(a);
    return 0;
}