#include <stdio.h>
int main(){
    float math = 9, english =  4;
    float avg = (math + english) / 2;
    if (avg >= 8){
        if (math >= 6.5 && english >= 6.5){
            printf("Excellent \n");
        }
        else if(math >= 5 && english >= 5){
            printf("Good \n");
        }
        else{
            printf("Average \n");
        }
    }
    else if(avg >= 6.5){
        if (math >= 5 && english >= 5){
            printf("Good \n");
        }
        else{
            printf("Average \n");
        }
    }
    else{
        printf("Average \n");
    }
}