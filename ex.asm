;ana are mere
.stack 
.data
	#var1 db 10
	#var2 db 1234h ;acestea sunt variabile
.code
*rut:
	ADD R0,200
	RETI
*start:
	PUSH PC
	ADD R0,200
	ADD R1,300
	SUB R1,299
	NOP
	MOV R2,(R1)10
	SEC
	BCC start
	NOP
	MOV R3,16
	SEV
	CLV
	BR start
end