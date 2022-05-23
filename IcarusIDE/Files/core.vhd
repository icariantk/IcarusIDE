library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.std_logic_unsigned.all;
use IEEE.std_logic_arith.all;


entity Core is
   Generic(
   BitsDeDatos:integer:=32;
   BitsDeDirecciones:integer:=32
   );
   Port ( Datos : inout  STD_LOGIC_VECTOR (BitsDeDatos-1 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
	rst:in std_logic;
   Direccion : out  STD_LOGIC_VECTOR (BitsDeDirecciones-1 downto 0):=x"00000000";
   clk:in std_logic;
   WE : out  STD_LOGIC:='0');
end Core;


architecture Behavioral of Core is
   begin
	
      process (clk,rst) is
      variable estado:integer range 0 to 15:=0;
      variable posicion:std_logic_vector(BitsDeDirecciones-1 downto 0);
      variable dedonde:std_logic_vector(BitsDeDirecciones-1 downto 0);
      variable adonde:std_logic_vector(BitsDeDirecciones-1 downto 0);
      variable amover:std_logic_Vector(BitsDeDatos-1 downto 0);

      begin
		if rst='0' then
         if clk'event and clk='1' then
			
				if estado=10 then
               estado:=0;
					we<='0';
            end if;
				if estado=9 then
					estado:=10;
					datos<=amover;
					
				end if;
            if estado=8 then
               we<='1';
               amover:=datos;
               
					direccion<=adonde;

					estado:=9;
            end if;
				if estado=7 then
				estado:=8;
				end if;
            if estado=6 then
               we<='0';
               adonde:=DATOS;
					direccion<=dedonde;
               estado:=7;

            end if;
				if estado=5 then
				estado:=6;
				end if;
            if estado=4 then
               we<='0';
               dedonde:=datos;
               direccion<=posicion+x"00000001";
               estado:=5;
            end if;
				if estado=3 then
				estado:=4;
				end if;
            if  estado=2 then
               we<='0';
               posicion:=datos;
               direccion<=posicion;
               estado:=3;
            end if;
				if estado=1 then
				estado:=2;
				end if;
            if estado=0 then
				   DATOS<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
               direccion<=x"00000000";
               we<='0';
               estado:=1;
            end if;

         end if;
			else
			direccion<=x"00000000";
			DATOS<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
			we<='0';
         estado:=0;
			end if;
      end process;
   end Behavioral;