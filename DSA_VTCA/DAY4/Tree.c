#include <stdio.h>
typedef struct Node{
        int value;
        struct Node* left;
        struct Node* right;
} Node;
typedef struct Queue{
    Node** array;
    int front;
    int rear;
    int capicity;
} Queue;
Node* createNode(int value){
    Node* node = (Node*)malloc(sizeof(Node));
    node->value = value;
    node->right = NULL;
    node->left = NULL;
    return node;
}
Queue* createQueue(int capicity){
    Queue* queue = (Queue*)malloc(sizeof(Queue));
    queue->capicity = capicity;
    queue->front = 0;
    queue->rear = -1;
    queue->array = (Node**)malloc(queue->capicity* sizeof(Node*));
    return queue;
}
int isEmpty(Queue *queue){
    return queue->front > queue->rear;
}
int enqueue(Queue *queue, Node* node){
    queue->array[++queue->rear] = node;
}
Node* dequeue(Queue* queue){
    return queue->array[queue->front++]
}
void preOrderTraversal(Node* root){
    if (root != NULL){
        printf("%d", root->value);
        preOrderTraversal(root->left);
        preOrderTraversal(root->right);
    }
} //Duyệt từ root đến left rồi đến right (theo bảng chữ cái)
void inOrderTraversal(Node* root){
    if (root != NULL){
        inOrderTraversal(root->left);
        printf("%d", root->value);
        inOrderTraversal(root->right);
    }
} //Duyệt từ nút con nhất -> nút nhánh link nút con đó -> nút con khác (nếu có) theo thứ tự left đến root rồi right
void postOrderTraversal(Node* root){
    if (root != NULL){
        postOrderTraversal(root->left);
        postOrderTraversal(root->right);
        printf("%d", root->value);
    }
} //Duyệt từ nút con nhất theo thứ tự left -> right -> root 
void deleteTree(Node* root){
    if (root != NULL){
        deleteTree(root->left);
        deleteTree(root->right);
        free(root);
    }
}
void defineTree(Node *root){
    //wait a minute kk
}
Node* insert(Node *root, int value){
    if (root == NULL) return createNode(value);
    if (value < root->value) root->left = insert(root->left, value);
    else if (value > root->value) root->right = insert(root->right, value);
    return root;
}
Node* search (Node* root, int value){
    if (root == NULL || root->value == value) return root;
    if (value < root->value) return search(root->left, value);
    else return search (root->right, value);
} //Nếu số cần tìm nhỏ hơn root => Duyệt sang trái, Lớn hơn thì duyệt sang phải
Node* findMin(Node *root){
    while (root->left != NULL) root = root->left;
    return root;
}
Node *findMax(Node *root){
    while (root->right != NULL) root = root->right;
    return root;
}
//Công thức tìm Max, Min: Nhỏ thường nhánh bên trái, lớn thường ở nhánh phải
Node *deleteNode (Node* root, int value){
    if (root == NULL) return root;
    if (value < root->value) root->left = deleteNode(root->left, value);
    else if (value > root->value) root->right = deleteNode(root->right, value);
    else{
        if (root->left == NULL){
            Node* temp = root->right;
            free(root);
            return temp;
        }
        else if (root->right === NULL){
            Node* temp = root->left;
            free(root);
            return temp;
        }
        Node* temp = findMin(root->right);
        root->value = temp->value;
        root->right = deleteNode(root->right, temp->value);
    }
} //Áp dụng như tìm kiểm, kiểm tra xem ptu cần xóa là lớn hay nhỏ hơn root. Nếu nhỏ thì tìm bên trái, lớn thì bên phải.
int depth(Node *root){
    if (root == NULL) return 0;
    int leftDepth = depth(root->left);
    int rightDepth = depth(root->right);
    return (leftDepth > rightDepth ? leftDepth: rightDepth) + 1;
}
int isBalanced (Node *root){
    if (root == NULL) return 1;
    int leftDepth = depth(root->left);
    int rightDepth = depth(root->right);
    return abs(leftDepth - rightDepth) <= 1&& isBalanced(root->left) && isBalanced(root->right);
} //độ sâu của bên trái trừ bên phải = 0 hoặc 1 => cây cân bằng.
//in cấp nhị phân theo cấp (ch làm được)

void levelOrderTraversal(Node* root){
    if (root == NULL) return;
    Queue* queue = createQueue(100);
    enqueue(queue, root);
    while (!isEmpty(queue)){
        Node* current = dequeue(queue);
        printf("%d", current->value);
        if (current->left != NULL) enqueue(queue, current->left);
        if (current->right != NULL) enqueue(queue, current->right);
    }
}
int sumTree(Node* root){
    if (root == NULL) return 0;
    return root->value + sumTree(root->left) + sumTree(root->right);
}
int areIdentical(Node* root1, Node* root2){
    if (root1 == NULL && root2 == NULL) return 1;
    if (root1 == NULL || root2 == NULL) return 0;
    return (root1->value == root2->value) && areIdentical(root1->left, root2->left) && areIdentical(root1->right, root2->right);
}
int countNodes(Node* root){
    if (root == NULL) return 0;
    return 1 + countNodes(root->left) + countNodes(root->right);
}
int isBSTUTil(Node* root, int min, int max){
    if (root == NULL) return 1;
    if (root->value < min || root->value > max) return 0;
    return isBSTUtil(root->left, min, root->value - 1) && isBSTUtil(root->right, root->value + 1, max);
}
int isBST(Node* root){
    return isBSTUtil(root, INT_MIN, INT_MAX);
} //đoạn này chưa hiểu + có 2 phần chưa làm xong.
int main(int argc, char const *argv[])
{
    return 0;
}
