#include <stdio.h>
int main(){
    int array[10] = {10, 21, 32, 43, 54, 65, 76, 87, 98, 99};
    int i;
    for (i = 0; i < 10; i++){
        printf("\n i = %d, array[i] = %d, *(array+i) = %d", i, array[i], *(array + i));
        printf(" &array[i] = %X, array + i = %X", &array[i], array + i);
    }
}