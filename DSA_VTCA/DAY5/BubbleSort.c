#include <stdio.h>
void bubbleSort(int arr[], int n){
    for (int i = 0; i < n-1; i++ ){
        for (int j = 0; j < n-i-1; j++){
            if (arr[j] > arr[j + 1]){
                int temp = arr[j];
                arr[j] = arr[j+1];
                arr[j+1] = temp;
            }
        }
    }
}
int findMax(int arr[], int n){
    int max = arr[0];
    for (int i = 1; i < n; i++){
        if (arr[i] > max){
            max = arr[i];
        }
    }
    return max;
}
void printArray(int arr[], int n){
    for (int i = 0; i < n; i++)
        printf("%d ", arr[i]);
    printf("\n");
}
void insertElement(int arr[], int *n, int pos, int value){
    for (int i = *n; i >= pos; i--){
        arr[i] = arr[i-1];
    }
    arr[pos-1] = value;
    (*n)++;
}
void deleteElement(int arr[], int *n, int pos){
    for (int i = pos-1; i < *n-1; i++){
        arr[i] = arr[i+1];
    }
    (*n)--;
}
int main(){
    int arr[] = {64, 34, 25, 12, 22, 11, 35};
    int n = sizeof(arr)/sizeof(arr[0]);
    bubbleSort(arr, n);
    printf("After sorting: ");
    printArray(arr, n);
    printf("Max = %d", findMax(arr, n));
    insertElement(arr, &n, 3, 199);
    printf("Array after inserting: ");
    printArray(arr, n);
    deleteElement(arr, &n, 3);
    printf("Array after deleting: ");
    printArray(arr, n);
    return 0;
}
