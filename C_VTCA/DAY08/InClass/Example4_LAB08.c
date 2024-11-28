#include <stdio.h>
#include <stdlib.h>
int main(){
    int *a;
    int count, i;
    printf("Input array size: ");
    scanf("%d", &count);
    a = (int*)calloc(count,sizeof(int));
    for (i = 0; i < count; i++){
        printf("a[%d] = ", i);
        scanf("%d", a + i);
    }
    a = (int*)realloc(a, (count + 1)*sizeof(int));
    printf("Input new element: ");
    scanf("%d", &a[count]);
    count++;
    printf("\nArray: \n");
    for (i = 0; i < count; i++){
        printf("%d", a[i]);
    }
    free(a);
    return 0;
}