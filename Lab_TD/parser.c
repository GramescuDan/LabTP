#include "parser.h"

AT_COMMAND_DATA mydata;
STATE_MACHINE_RETURN_VALUE at_command_parse(uint8_t current_character)
{
    static uint32_t state = 0;
    switch (state)
    {
        case 0:
        {
            printf("state 0\n");
            if (current_character == '\r')
            {
                printf("cr");
                state = 1;
            }
            break;
        }
        case 1:
        {
            if (current_character == '\n')
            {
                state = 2;
                printf("lf");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }
        case 2:
        {
            if (current_character == 'O')
            {
                state = 3;
                printf("o");
            }
            else if (current_character == 'E')
            {
                state = 7;
                printf("e");
            }
            else if (current_character == '+')
            {
                state = 14;
                printf("+");
            }

            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }
        case 3:
        {
            if (current_character == 'K')
            {
                state = 4;
                printf("k");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 4:
        {
            if (current_character == 0x0D)
            {
                state = 5;
                printf("cr");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 5:
        {
            if (current_character == 0x0A)
            {
                return STATE_MACHINE_READY_OK;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 7:
        {
            if (current_character == 'R')
            {
                state = 8;
                printf("r");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 8:
        {
            if (current_character == 'R')
            {
                state = 9;
                printf("r");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 9:
        {
            if (current_character == 'O')
            {
                state = 10;
                printf("");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 10:
        {
            if (current_character == 'R')
            {
                state = 11;
                printf("");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 11:
        {
            if (current_character == 0x0D)
            {
                state = 12;
                printf("");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 12:
        {
            if (current_character == 0x0A)
            {
                return STATE_MACHINE_READY_OK;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 14:
        {
            if (current_character > 0x20 && current_character < 0x7F)
            {
                state = 15;
                printf("");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 15:
        {
            if (current_character == 0X0D)
            {
                state = 16;
                printf("");
            }
            else if (current_character > 0x20 && current_character < 0x7F)
            {
                state = 15;
                printf("");
            }

            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 16:
        {
            if (current_character == 0x0A)
            {
                state = 17;
                printf("");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 17:
        {
            if (current_character == 0x0D)
            {
                state = 18;
                printf("");
            }
            else if(current_character == '+')
            {
                state = 14;
                printf("");
            }

            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 18:
        {
            if (current_character == 0x0A)
            {
                state = 19;
                printf("");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 19:
        {
            if (current_character == 'O')
            {
                state = 3;
                printf("");
            }
            else if (current_character == 'E')
            {
                state = 7;
                printf("");
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

    }
    return STATE_MACHINE_NOT_READY;
}

void  fun(){

    FILE *f;
    unsigned char c;

    if((f=fopen("program","rb"))==NULL){
        printf("eroare deschidere fisier");
    }
    while(fscanf(f,"%c",&c)==1){

        STATE_MACHINE_RETURN_VALUE val = at_command_parse(c);

        if(val == STATE_MACHINE_NOT_READY) {
            printf("n-are baterie");
        }

        if(val == STATE_MACHINE_READY_OK) {
            printf("s-a citit");

        }

        if(val == STATE_MACHINE_READY_WITH_ERROR) {
            printf("eroare citire");
        }
        break;

    }
}