#include "parser.h"

int main(int argc, char * argv[]){

    FILE *f;
    unsigned char c;
    int flag=0;
    unsigned char prev;

    if((f=fopen(argv[1],"rb"))==NULL){
        printf("eroare deschidere fisier");
    }
    while((flag = fscanf(f,"%c",&c)) !=EOF){

        STATE_MACHINE_RETURN_VALUE val = parseNextChar(c);
        if(val == STATE_MACHINE_READY_OK && flag == 1) {

            for(int i = 0;i<1000;i++){
                printf("%d: %s\n",i,mydata.data[i]);
            }

        }

        else if(val == STATE_MACHINE_READY_WITH_ERROR) {
            break;
        }

        else if(val == STATE_MACHINE_READY_OK && flag != 1){
            exit(EXIT_SUCCESS);
        }

    }
}