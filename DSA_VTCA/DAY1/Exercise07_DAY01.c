#include <stdio.h>
int main(){
    int year;
    printf("Input a year: ");
    scanf("%d", &year);
    if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0){
        //năm có đuôi 00 thì chia cho 400, còn lại là trường hợp còn lại.
        printf("%d là năm nhuận \n", year);
    }
    else{
        printf("%d không phải là năm nhuận \n", year);
    }

}