library ieee;
use ieee.std_logic_1164.ALL;
use ieee.numeric_std.ALL;
library UNISIM;
use UNISIM.Vcomponents.ALL;

entity Puerto_Serial is
   port ( clk       : in    std_logic; 
          clk_fast  : in    std_logic; 
          Direccion : in    std_logic_vector (1 downto 0); 
          enable    : in    std_logic; 
          Uart_rx   : in    std_logic; 
          WE    : in    std_logic; 
          Uart_tx   : out   std_logic; 
          Datos     : inout std_logic_vector (31 downto 0));
end Puerto_Serial;

architecture BEHAVIORAL of Puerto_Serial is
   signal XLXN_7    : std_logic_vector (31 downto 0);
   signal XLXN_8    : std_logic_vector (31 downto 0);
   signal XLXN_10   : std_logic_vector (31 downto 0);
   signal XLXN_11   : std_logic_vector (31 downto 0);
   component PuertoSerial
      port ( Enable    : in    std_logic; 
             clk       : in    std_logic; 
             WE        : in    std_logic; 
             Direccion : in    std_logic_vector (1 downto 0); 
             rx        : in    std_logic_vector (31 downto 0); 
             estados   : in    std_logic_vector (31 downto 0); 
             Datos     : inout std_logic_vector (31 downto 0); 
             tx        : out   std_logic_vector (31 downto 0); 
             comando   : out   std_logic_vector (31 downto 0));
   end component;
   
   component Serial
      port ( Uart_rx : in    std_logic; 
             CLK     : in    std_logic; 
             TX      : in    std_logic_vector (31 downto 0); 
             Comando : in    std_logic_vector (31 downto 0); 
             Uart_tx : out   std_logic; 
             RX      : out   std_logic_vector (31 downto 0); 
             Estados : out   std_logic_vector (31 downto 0));
   end component;
   
begin
   Interfaz : PuertoSerial
      port map (clk=>clk,
                Direccion(1 downto 0)=>Direccion(1 downto 0),
                Enable=>enable,
                estados(31 downto 0)=>XLXN_8(31 downto 0),
                rx(31 downto 0)=>XLXN_7(31 downto 0),
                WE=>WE,
                comando(31 downto 0)=>XLXN_10(31 downto 0),
                tx(31 downto 0)=>XLXN_11(31 downto 0),
                Datos(31 downto 0)=>Datos(31 downto 0));
   
   PSerial : Serial
      port map (CLK=>clk_fast,
                Comando(31 downto 0)=>XLXN_10(31 downto 0),
                TX(31 downto 0)=>XLXN_11(31 downto 0),
                Uart_rx=>Uart_rx,
                Estados(31 downto 0)=>XLXN_8(31 downto 0),
                RX(31 downto 0)=>XLXN_7(31 downto 0),
                Uart_tx=>Uart_tx);
   
end BEHAVIORAL;

library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity PuertoSerial is
    Port ( Datos : inout  STD_LOGIC_VECTOR (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
			  Direccion : in std_logic_Vector(1 downto 0);
			  Enable : in  STD_LOGIC;
			  tx: out std_logic_Vector (31 downto 0):=x"00000000";
			  comando: out std_logic_Vector (31 downto 0):=x"00000000";
			  rx: in std_logic_Vector (31 downto 0);
			  estados: in std_logic_Vector (31 downto 0);
			  clk:in std_logic;
           WE : in  STD_LOGIC);
end  PuertoSerial;

architecture Behavioral of  PuertoSerial is
begin

process (clk) is
variable r_tx:std_logic_vector(31 downto 0):=x"00000000";
variable r_rx:std_logic_vector(31 downto 0):=x"00000000";
variable r_cmd:std_logic_vector(31 downto 0):=x"00000000";
variable r_est:std_logic_vector(31 downto 0):=x"00000000";
begin
--00   comando
--01   status
--10   rx
--11   tx

if clk'event and clk='1' then
if enable='1' then
    if we='1' then
	   if direccion="00" then
				 r_cmd:=x"00000000" or datos;
		end if;
		if direccion="11" then
				 r_tx:=x"00000000" or datos;
		end if;
	 else
	   if direccion="00" then
				 datos<=r_cmd;
		end if;
		if direccion="01" then
				 datos<=r_est;
		end if;
	   if direccion="10" then
				 datos<=r_rx;
		end if;
		if direccion="11" then
				 datos<=r_tx;
		end if;		
	 end if;


else
  datos<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
  tx<=r_tx;
  comando<=r_cmd;
  r_rx:=rx;
  r_est:=estados;

end if;
end if;
end process;

end Behavioral;

library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.std_logic_arith.all;

--<TX> registro a enviar
--<RX> registro donde recibir
--
--<Comando> registro de control del puerto serial
--comando(0) enviar
--comando(1) limpiar recibidos
--comando(3 downto 2)   numero de datos a enviar
--
--<Estados> registro de status del puerto serial
--estados(31 downto 30) enviando dato numero XX
--estados(29 downto 28) recibiendo dato numero XX
--estados(27) enviando
--estados(26) recibiendo
--estados(25) dato en TX
--estados(24) dato en RX
--
--<Uart_tx> puerto serial de salida de datos
--
--<Uart_rx> puerto serial de entrada de datos
--
--<clk> entrada de reloj
--
--<espera> generic que debe calcularse (frecuencia del reloj/baudios requeridos)
entity Serial is
generic(
	espera:integer:=434 --Pulsos necesarios para generar 115,200 baudios
);
    Port ( TX : in  STD_LOGIC_VECTOR (31 downto 0);
           RX : out  STD_LOGIC_VECTOR (31 downto 0);
           Estados : out  STD_LOGIC_VECTOR (31 downto 0);
           Comando : in  STD_LOGIC_VECTOR (31 downto 0);
           Uart_tx : out  STD_LOGIC;
           Uart_rx : in  STD_LOGIC;
           CLK : in  STD_LOGIC);
end Serial;

architecture Behavioral of Serial is

begin
estadoS(24)<=uart_rx;
process (clk) is --entrada de datos
variable numero:integer range 0 to 3:=0;
variable estado:integer range 0 to 63:=0;
variable pasito:integer:=0;
variable registro_entrada:std_logic_Vector(39 downto 0):=x"0000000000";
begin
if clk'event and clk='1' then
   estadoS(29 downto 28)<=CONV_STD_LOGIC_VECTOR(numero, 2);
   if comando(1) = '1' then
		numero:=0;
	end if;
   if estado=11 then
		estadoS(26)<='1';
		if numero < 3 then
			numero:=numero+1;
		end if;
	   estado:=0;
		pasito:=0;
		rx<=registro_entrada(38 downto 31)&registro_entrada(28 downto 21)&registro_entrada(18 downto 11)&registro_entrada(8 downto 1);
	end if;
   if pasito=espera then
		estado:=estado+1;
		pasito:=0;
	end if;
   if pasito=espera/2 then
		registro_entrada:=Uart_rx&registro_entrada(39 downto 1);
	end if;
	if estado>0 then
	   pasito:=pasito+1;
	end if;
   if estado=0 and Uart_rx='0' then
	   estadoS(26)<='1';
		estado:=1;
	end if;
end if;
end process;
process (clk) is --Envio de datos
variable numero:integer range 0 to 3:=0;
variable estado:integer range 0 to 63:=0;
variable pasito:integer:=0;
variable r_salida:std_logic_Vector(39 downto 0):=x"1111111111";
begin
if clk'event and clk='1' then
estadoS(25)<=r_salida(0);
estadoS(31 downto 30)<=CONV_STD_LOGIC_VECTOR(numero, 2);
uart_tx<=r_salida(0);
if comando(3 downto 2)="00" and estado=11 then
	estadoS(27)<='0';
	estado:=0;
	numero:=0;
end if;
if comando(3 downto 2)="10" and estado=31 then
	estadoS(27)<='0';
	estado:=0;
	numero:=0;
end if;
if comando(3 downto 2)="01" and estado=21 then
	estadoS(27)<='0';
	estado:=0;
	numero:=0;
end if;
if estado=10 or estado=20 or estado=30 or estado=40 then
  numero:=numero+1;
end if;
if comando(3 downto 2)="11" and estado=41 then
	estadoS(27)<='0';
	estado:=0;
	numero:=0;
end if;

if pasito=espera then
pasito:=0;
estado:=estado+1;
r_salida:='1'&r_salida(39 downto 1);
end if;
if estado>0 then
	pasito:=pasito+1;
end if;
if estado=0 and comando(0)='1' then
estadoS(27)<='1';
if comando(3 downto 2)="00" then
	r_salida:=x"1111111"&"111"&tx(7 downto 0)&'0';
end if;
if comando(3 downto 2)="10" then
	r_salida:=x"11"&"111"&tx(23 downto 16)&"01"&tx(15 downto 8)&"01"&tx(7 downto 0)&'0';
end if;
if comando(3 downto 2)="01" then
	r_salida:=x"11111"&"1"&tx(15 downto 8)&"01"&tx(7 downto 0)&'0';
end if;
if comando(3 downto 2)="11" then
	r_salida:='1'&tx(31 downto 24)&"01"&tx(23 downto 16)&"01"&tx(15 downto 8)&"01"&tx(7 downto 0)&'0';
end if;
estado:=1;
end if;
end if;
end process;
end Behavioral;



