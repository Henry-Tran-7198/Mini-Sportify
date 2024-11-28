#include <stdio.h>
int main(){
    int myArray[5] = {1, 5, 7, 8, 9};
    int i;
    
    for (i = 0; i < 5; i++){
        int* ptr = &myArray[i];
        printf("Mảng là: \n %d ", *ptr);
    }
}