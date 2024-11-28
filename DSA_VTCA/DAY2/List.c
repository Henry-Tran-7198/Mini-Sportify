#include <stdio.h>
#include <stdlib.h>
#define MAX 100
typedef struct{
    int data[MAX];
    int size;
} List;
void initList(List *list){
    list->size = 0;
}
void addList(List *list, int value){
    if (list->size < MAX){
        list->data[list->size++] = value;
    }
    else{
        printf("List is full \n");
    }
}
void printList(List *list){
    for (int i = 0; i < list->size; i++){
        printf("%d ", list->data[i]);
    }
    printf("\n");
}
void deleteList(List *list, int index){
    if (index < 0 || index >= list->size){
        printf("Index out of bounds \n");
        return;
    }
    for (int i = index; i < list->size - 1; i++){
        list->data[i] = list->data[i + 1];
    }
    list->size--;
}
void insertList(List *list, int index, int value){
    if (index < 0 || index > list->size || list->size >= MAX){
        printf("Index out of bounds or List is full. \n");
        return;
    }
    for (int i = list->size; i > index; i--){
        list->data[i] = list->data[i-1];
    }
    list->data[index] = value;
    list->size++;
}
int find(List *list, int value){
    for (int i = 0; i < list->size; i++){
        if (list->data[i] == value){
            return i;
        }
    }
    return -1;
}
void sortList(List *list){
    for (int i = 0; i < list->size - 1; i++){
        for (int j = i + 1; j < list->size; j++){
            if (list->data[i] > list->data[j]){
                int tmp = list->data[i];
                list->data[i] = list->data[j];
                list->data[j] = tmp;
            }
        }
    }
}
void reverseList(List *list){
    int start = 0;
    int end = list->size - 1;
    while (start < end){
        int tmp = list->data[start];
        list->data[start] = list->data[end];
        list->data[end] = tmp;
        start++;
        end--;
    }    
}
void mergeLists(List *list1, List *list2, List *result){
    initList(result);
    for (int i = 0; i < list1->size; i++){
        add(result, list1->data[i]);
    }
    for (int i = 0; i < list2->size; i++){
        add(result, list2-> data[i]);
    }
}
void copyList(List *source, List *destination){
    initList(destination);
    for (int i = 0; i < source->size; i++){
        add(destination, source->data[i]);
    }
}
void clearList(List *list){
    list->size = 0;
}
int main(int argc, char const *argv[])
{
    List list;
    printf("Bài 1 \n");
    initList(&list);
    addList(&list, 10);
    addList(&list, 20);
    addList(&list, 90);
    printf("List: \n");
    printList(&list);
    printf("Bài 2 \n");
    initList(&list);
    addList(&list, 10);
    addList(&list, 20);
    addList(&list, 90);
    printf("List before deleting: \n");
    printList(&list);
    printf("List after deleting: \n");
    deleteList(&list, 1);
    printList(&list);
    printf("Bài 3 \n");
    initList(&list);
    addList(&list, 10);
    addList(&list, 20);
    addList(&list, 90);
    printf("List before inserting an element: \n");
    printList(&list);
    printf("List after inserting an element: \n");
    insertList(&list, 1, 19000);
    printList(&list);
    printf("Bài 4 \n");
    initList(&list);
    addList(&list, 10);
    addList(&list, 20);
    addList(&list, 90);
    printList(&list);
    int index = find(&list, 90);
    if (index != -1){
        printf("Element was found at index %d", index);
    }
    else{
        printf("Element not found! ");
    }
    printf("Bài 5 \n");
    initList(&list);
    addList(&list, 70);
    addList(&list, 20);
    addList(&list, 90);
    printf("List before sorting: \n");
    printList(&list);
    printf("List after sorting: \n");
    sortList(&list);
    printf(&list);
    printf("Bài 6 \n");
    initList(&list);
    addList(&list, 150);
    addList(&list, 200);
    addList(&list, 50);
    addList(&list, 300);
    printf("List before reversing: \n");
    printList(&list);
    printf("List after reversing: \n");
    reverseList(&list);
    printList(&list);
    printf("Bài 7 \n");
    List list1, list2, merged;
    initList(&list1);
    initList(&list2);
    addList(&list1, 150);
    addList(&list2, 200);
    addList(&list1, 50);
    addList(&list2, 300);
    printf("List 1: \n");
    printList(&list1);
    printf("List 2: \n");
    printList(&list2);
    printf("List after merged with 1 and 2: ");
    mergeLists(&list1, &list2, &merged);
    printList(&merged);
    printf("Bài 8: \n");
    List original, copy;
    initList(&original);
    addList(&original, 120);
    addList(&original, 50);
    addList(&original, 190);
    copyList(&original, &copy);
    printf("List copied from original: \n");
    printList(&copy);
    printf("Bài 9: \n");
    initList(&list);
    addList(&list, 20);
    addList(&list, 50);
    printf("List before clearing: \n");
    printList(&list);
    printf("List after clearing: \n");
    clearList(&list);
    printList(&list);
    return 0;
}
