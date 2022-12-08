#include <stdint.h>
#include <assert.h>
#include <string.h>
#include <stdio.h>
#include "LPC177x_8x.h"
#include "system_LPC177x_8x.h"
#include <retarget.h>

#include <DRV\drv_sdram.h>
#include <DRV\drv_lcd.h>
#include <DRV\drv_uart.h>
#include <DRV\drv_touchscreen.h>
#include <DRV\drv_led.h>
#include <utils\timer_software.h>
#include <utils\timer_software_init.h>

#include "parser.h"

AT_COMMAND_DATA data_structure;
const char* at_command_simple = "AT\r\n";
const char* at_command_csq = "AT+CSQ\r\n";
const char* at_command_gmi = "AT+GMI\r\n";
const char* at_command_gmr = "AT+GMR\r\n";
const char* at_command_gmm = "AT+GMM\r\n";
const char* at_command_creg = "AT+CREG\r\n";
const char* at_command_cops = "AT+COPS\r\n";
const char* at_command_gsn = "AT+GSN\r\n";

timer_software_handler_t my_timer_handler;
timer_software_handler_t my_handler;

static BOOLEAN flag;

void timer_callback_1(timer_software_handler_t h)
{
}

void testLCD()
{
	uint32_t i,j;
	LCD_PIXEL foreground = {0, 255, 0, 0};
	LCD_PIXEL background = {0, 0, 0, 0};
	
	
	for (i = 0; i < LCD_HEIGHT; i++)
	{
		for (j = 0; j < LCD_WIDTH / 3; j++)
		{
			DRV_LCD_PutPixel(i, j, 255, 0, 0);
		}
		for (j = LCD_WIDTH / 3; j < 2 * (LCD_WIDTH / 3); j++)
		{
			DRV_LCD_PutPixel(i, j, 230, 220, 0);
		}
		for (j = 2 * (LCD_WIDTH / 3); j < LCD_WIDTH; j++)
		{
			DRV_LCD_PutPixel(i, j, 0, 0, 255);
		}
	}

	DRV_LCD_Puts("Hello", 20, 30, foreground, background, TRUE);
	DRV_LCD_Puts("Hello", 20, 60, foreground, background, FALSE);	
}

void TouchScreenCallBack(TouchResult* touchData)
{		
}

void BoardInit()
{
	timer_software_handler_t handler;
	
	TIMER_SOFTWARE_init_system();
	
	
	DRV_SDRAM_Init();
	
	initRetargetDebugSystem();
	DRV_LCD_Init();
	DRV_LCD_ClrScr();
	DRV_LCD_PowerOn();	
	
	DRV_TOUCHSCREEN_Init();
	DRV_TOUCHSCREEN_SetTouchCallback(TouchScreenCallBack);
	DRV_LED_Init();	
	
	handler = TIMER_SOFTWARE_request_timer();
	TIMER_SOFTWARE_configure_timer(handler, MODE_1, 500, 1);
	TIMER_SOFTWARE_set_callback(handler, timer_callback_1);
	TIMER_SOFTWARE_start_timer(handler);
}

void MyTimerCallback(timer_software_handler_t handler)
{
	DRV_LED_Toggle(LED_1);
} 

void GetCommandResponse(const char *command)
{
 unsigned char ch;
 int flag = 1;
 BOOLEAN ready = FALSE;
 TIMER_SOFTWARE_reset_timer(my_handler);
 TIMER_SOFTWARE_start_timer(my_handler);

 while ((!TIMER_SOFTWARE_interrupt_pending(my_handler)) && (ready == FALSE))
 {
	 while (DRV_UART_BytesAvailable(UART_3) > 0)
	 {
		 STATE_MACHINE_RETURN_VALUE retval;
		 DRV_UART_ReadByte(UART_3, &ch);
		
		 retval = parseNextChar(ch,flag);
		 if (retval == STATE_MACHINE_NOT_READY)
		 {
		 
		 }
				if(retval == STATE_MACHINE_READY_OK){
					flag =TRUE;
					ready =TRUE;
					break;
				}
				if(retval == STATE_MACHINE_READY_WITH_ERROR){
					
					flag = FALSE;
					ready =TRUE;
					break;
					
				}
	 }
 }
} 

void SendCommand(const char *command)
{
 DRV_UART_FlushRX(UART_3);
 DRV_UART_FlushTX(UART_3);
 DRV_UART_Write(UART_3, (uint8_t*) command, strlen(command));
} 


void ExecuteCommand(const char *command)
{
 SendCommand(command);
 GetCommandResponse(command);
} 

int ConvertAsuToDbmw(int number)
{
	return number*2 -113;
}

int ExtractData()
{
		int i = 0;
		int number = 0;
		while(1)
		{
			if((data_structure.data[0][i] >= '0') && (data_structure.data[0][i] <= '9'))
			{
			
				number = number*10 + (data_structure.data[0][i] -'0');
			
			} 
			else if(number != 0) 
			{
				if (data_structure.data[0][i + 1] >= '5') 
				{
					number++;
					break;
				}
				else 
				{
					break;
				}
			}
			i++;
		}
		return number;
}
char* ExtractDataString(int j)
{
		return data_structure.data[j];
}


BOOLEAN CommandResponseValid()
{
	return flag;
}


int main(void)
{
	const char* rssi_value_asu;
	int rssi_value_dbmw; 
	
	timer_software_handler_t handler;
	BoardInit();

	
	
	/* configure the requested timer to run in MODE_1 (reset and restart at match)
	with a period of 15 s*/
	
	DRV_UART_Configure(UART_3, UART_CHARACTER_LENGTH_8, 115200, UART_PARITY_NO_PARITY, 1, TRUE, 3);
	
	DRV_UART_Write(UART_3,(uint8_t*) at_command_simple, strlen(at_command_simple));
	TIMER_SOFTWARE_Wait(1000); 
	DRV_UART_Write(UART_3, (uint8_t*)at_command_simple, strlen(at_command_simple));
	TIMER_SOFTWARE_Wait(1000); 
	DRV_UART_Write(UART_3, (uint8_t*)at_command_simple, strlen(at_command_simple));
	TIMER_SOFTWARE_Wait(1000);
		//user code
	
	 TIMER_SOFTWARE_init_system(); // initialize the software timer library
	  // declare a software timer handler
	 my_timer_handler = TIMER_SOFTWARE_request_timer(); // request a timer
	 if (my_timer_handler < 0) // check if the request was successful
	 {
	 // the system could not offer a software timer
	 }
	 /*configure the requested timer to run in MODE_1 (reset and restart at match)
	with a period of 1 s*/
	
	 TIMER_SOFTWARE_configure_timer(my_timer_handler, MODE_1, 2000, true);
	 // set a callback for the requested timer
	 TIMER_SOFTWARE_start_timer(my_timer_handler);
	 
	 
	 
	 my_handler = TIMER_SOFTWARE_request_timer(); // request a timer
	 if (my_handler < 0) // check if the request was successful
	 {
	 // the system could not offer a software timer
	 }
	 TIMER_SOFTWARE_configure_timer(my_handler, MODE_1, 15000, true);
	 while(1)
	 {
		 if (TIMER_SOFTWARE_interrupt_pending(my_timer_handler))
		 {
			 TIMER_SOFTWARE_Wait(500);
			 ExecuteCommand(at_command_gmi);
			 ExecuteCommand(at_command_gmr);
			 ExecuteCommand(at_command_gmm);
			 if (CommandResponseValid()== TRUE)
			 {
				 rssi_value_asu = ExtractDataString(1);
				 //rssi_value_dbmw = ConvertAsuToDbmw(rssi_value_asu);
				 //printf("GSM Modem signal %d ASU -> %d dBmW\n", rssi_value_asu, rssi_value_dbmw);
				 printf("%s" ,rssi_value_asu);
			 }
			 
			 TIMER_SOFTWARE_clear_interrupt(my_timer_handler);
		 } 
	 }
	return 0; 
}


