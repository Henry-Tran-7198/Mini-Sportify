//Đọc 1 số trong phạm vi từ 0 đến 9
#include <stdio.h>
int main(){
    int number;
    printf("\n Nhập số bạn cần tìm từ 0 đến 9 đi: ");
    scanf("%d", &number);
    if (number < 0 || number > 9){
        printf("Không đúng số trong phạm vi 0 đến 9 \n");
    } else {
        switch (number){
            case 1: 
            printf("Số bạn nhập là số 1");
            break;
            case 2:
            printf("Số bạn nhập là số 2");
            break;
            case 3:
            printf("Số bạn nhập là số 3");
            break;
            case 4:
            printf("Số bạn nhập là số 4");
            break;
            case 5:
            printf("Số bạn nhập là số 5");
            break;
            case 6:
            printf("Số bạn nhập là số 6");
            break;
            case 7:
            printf("Số bạn nhập là số 7");
            break;
            case 8:
            printf("Số bạn nhập là số 8");
            break;
            default:
            printf("Số bạn nhập là số 9");
            break;
        }
    }
}