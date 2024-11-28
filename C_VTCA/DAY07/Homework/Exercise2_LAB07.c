#include <stdio.h>
#include <stdlib.h>
#include <time.h>
int main() {
    int arr[100], i, n = 100;
    srand(time(NULL));
    for (i = 0; i < n; i++){
        arr[i] = rand() % 1000;
    }
    printf("Array: \n");
    for (i = 0; i < n; i++){
        printf("%d ", arr[i]);
    }
    for (int i = 0; i < n - 1; i++) {
        for (int j = i + 1; j < n; j++) {
            if (arr[i] > arr[j]) {
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
    }
    printf("\nArray after sorting: ");
    for (int i = 0; i < n; i++) {
        printf("%d ", arr[i]);
    }
    
}