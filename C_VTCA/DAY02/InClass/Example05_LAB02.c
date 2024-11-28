#include <stdio.h>
int main(){
    int size;
    size = sizeof(int);
    printf("Int: %d bytes \n", size);
    size = sizeof(long int);
    printf("Long int: %d bytes \n", size);
    size = sizeof(double);
    printf("Double: %d bytes \n", size);
    size = sizeof(long double);
    printf("Long double: %d bytes \n", size);
}