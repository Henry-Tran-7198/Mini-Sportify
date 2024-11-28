#include <stdio.h>
int main(){
    int num;
    printf("Input int num: ");
    scan("%d", &num);
    if (num % 2 == 0){
        printf("%d là số chẵn \n", num);
    }
    else {
        printf("%d là số lẻ \n", num);
    }
    
}