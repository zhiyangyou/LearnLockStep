using System;


namespace ManualMD5
{



	/*******************************************************
	 * Programmed by
	 *			 syed Faraz mahmood 
	 *				Student NU FAST ICS
	 * can be reached at s_faraz_mahmood@hotmail.com
	 * 
	 * 
	 * 
	 * *******************************************************/

	/// <summary>
	/// Summary description for MD5.
	/// </summary>
	public class MD5
	{
		/***********************VARIABLES************************************/


		/***********************Statics**************************************/
		/// <summary>
		/// lookup table 4294967296*sin(i)
		/// </summary>
		protected readonly static uint []  T =new uint[64] 
			{	0xd76aa478,0xe8c7b756,0x242070db,0xc1bdceee,
				0xf57c0faf,0x4787c62a,0xa8304613,0xfd469501,
                0x698098d8,0x8b44f7af,0xffff5bb1,0x895cd7be,
                0x6b901122,0xfd987193,0xa679438e,0x49b40821,
				0xf61e2562,0xc040b340,0x265e5a51,0xe9b6c7aa,
                0xd62f105d,0x2441453,0xd8a1e681,0xe7d3fbc8,
                0x21e1cde6,0xc33707d6,0xf4d50d87,0x455a14ed,
				0xa9e3e905,0xfcefa3f8,0x676f02d9,0x8d2a4c8a,
                0xfffa3942,0x8771f681,0x6d9d6122,0xfde5380c,
                0xa4beea44,0x4bdecfa9,0xf6bb4b60,0xbebfbc70,
                0x289b7ec6,0xeaa127fa,0xd4ef3085,0x4881d05,
				0xd9d4d039,0xe6db99e5,0x1fa27cf8,0xc4ac5665,
                0xf4292244,0x432aff97,0xab9423a7,0xfc93a039,
                0x655b59c3,0x8f0ccc92,0xffeff47d,0x85845dd1,
                0x6fa87e4f,0xfe2ce6e0,0xa3014314,0x4e0811a1,
				0xf7537e82,0xbd3af235,0x2ad7d2bb,0xeb86d391};
		
		/*****instance variables**************/
		/// <summary>
		/// X used to proces data in 
		///	512 bits chunks as 16 32 bit word
		/// </summary>
		protected  uint [] X = new uint [16];	
		
		/// <summary>
		/// the finger print obtained. 
		/// </summary>
		protected Digest dgFingerPrint;			

		/// <summary>
		/// the input bytes
		/// </summary>
		protected	byte [] m_byteInput;		
		
	
		
		/**********************EVENTS AND DELEGATES*******************************************/

		public delegate void ValueChanging (object sender,MD5ChangingEventArgs Changing);
		public delegate void ValueChanged (object sender,MD5ChangedEventArgs Changed);


		public event ValueChanging OnValueChanging;
		public event ValueChanged  OnValueChanged;


		
		/********************************************************************/
		/***********************PROPERTIES ***********************/
		/// <summary>
		///gets or sets as string
		/// </summary>
		public string Value
		{
			get
			{ 
				string st ;
				char [] tempCharArray= new Char[m_byteInput.Length];
				
				for(int i =0; i<m_byteInput.Length;i++)
					tempCharArray[i]=(char)m_byteInput[i];

				st= new String(tempCharArray);
				return st;
			}
			set
			{
				/// raise the event to notify the changing 
				//if (this.OnValueChanging !=null)
				//	this.OnValueChanging(this,new MD5ChangingEventArgs(value));


				m_byteInput=new byte[value.Length];
				for (int i =0; i<value.Length;i++)
					m_byteInput[i]=(byte)value[i];
				dgFingerPrint=CalculateMD5Value();				

				/// raise the event to notify the change
				//if (this.OnValueChanged !=null)
				//	this.OnValueChanged(this,new MD5ChangedEventArgs(value,dgFingerPrint.ToString()));
				 
			}
		}		
		
		/// <summary>
		/// get/sets as  byte array 
		/// </summary>
		public byte [] ValueAsByte
		{
			get
			{
                return m_byteInput;
				//byte [] bt = new byte[m_byteInput.Length];
				//for (int i =0; i<m_byteInput.Length;i++)
				//	bt[i]=m_byteInput[i];
				//return bt;
            }

            set
            {
				/// raise the event to notify the changing
				//if (this.OnValueChanging !=null)
				//	this.OnValueChanging(this,new MD5ChangingEventArgs(value));

                m_byteInput = value;
				//m_byteInput=new byte[value.Length];
				//for (int i =0; i<value.Length;i++)
				//	m_byteInput[i]=value[i];
				dgFingerPrint=CalculateMD5Value();

				/// notify the changed  value
				//if (this.OnValueChanged !=null)
				//	this.OnValueChanged(this,new MD5ChangedEventArgs(value,dgFingerPrint.ToString()));
			}
		}

		//gets the signature/figner print as string
		public  string FingerPrint
		{
			get
			{
				return dgFingerPrint.ToString();
			}
		}

        public byte[] FingerBytes{
            get 
            {
                byte[] result  = new byte[16];

                _revertInt2Bytes(dgFingerPrint.A, result, 0 );
                _revertInt2Bytes(dgFingerPrint.B, result, 4 );
                _revertInt2Bytes(dgFingerPrint.C, result, 8 );
                _revertInt2Bytes(dgFingerPrint.D, result, 12);
        
                return result;
            }
        }

        private void _revertInt2Bytes(uint value, byte[] array, int offset)
        {
            if (null == array)
            {
                return;
            }

            if (offset + 4 > array.Length)
            {
                return ;
            }

            array[offset + 0] = (byte)((value & 0x000000ff)      ); 
            array[offset + 1] = (byte)((value & 0x0000ff00) >> 8 ); 
            array[offset + 2] = (byte)((value & 0x00ff0000) >> 16); 
            array[offset + 3] = (byte)((value & 0xff000000) >> 24); 
        }
    
        //public byte[] FingerBytes
        //{
        //    get {
        //        return dgFingerPrint;
        //    }
        //}



		/*************************************************************************/
		/// <summary>
		/// Constructor
		/// </summary>
		public MD5()
		{			
			Value="";
		}
		
		
		/******************************************************************************/
		/*********************METHODS**************************/

		/// <summary>
		/// calculat md5 signature of the string in Input
		/// </summary>
		/// <returns> Digest: the finger print of msg</returns>
		protected Digest CalculateMD5Value()
		{
			/***********vairable declaration**************/
			byte [] bMsg;	//buffer to hold bits
			uint N;			//N is the size of msg as  word (32 bit) 
			Digest dg =new Digest();			//  the value to be returned

			// create a buffer with bits padded and length is alos padded
			bMsg=CreatePaddedBuffer();

			N=(uint)(bMsg.Length*8)/32;		//no of 32 bit blocks
			
			for (uint  i=0; i<N/16;i++)
			{
				CopyBlock(bMsg,i);
				PerformTransformation(ref dg.A,ref dg.B,ref dg.C,ref dg.D);
			}
			return dg;
		}
	
		/********************************************************
		 * TRANSFORMATIONS :  FF , GG , HH , II  acc to RFC 1321
		 * where each Each letter represnets the aux function used
		 *********************************************************/



		/// <summary>
		/// perform transformatio using f(((b&c) | (~(b)&d))
		/// </summary>
		protected void TransF(ref uint a, uint b, uint c, uint d,uint k,ushort s, uint i )
		{
			a = b + MD5Helper.RotateLeft((a + ((b&c) | (~(b)&d)) + X[k] + T[i-1]), s);
		}

		/// <summary>
		/// perform transformatio using g((b&d) | (c & ~d) )
		/// </summary>
		protected void TransG(ref uint a, uint b, uint c, uint d,uint k,ushort s, uint i )
		{
			a = b + MD5Helper.RotateLeft((a + ((b&d) | (c & ~d) ) + X[k] + T[i-1]), s);
		}
		
		/// <summary>
		/// perform transformatio using h(b^c^d)
		/// </summary>
		protected void TransH(ref uint a, uint b, uint c, uint d,uint k,ushort s, uint i )
		{
			a = b + MD5Helper.RotateLeft((a + (b^c^d) + X[k] + T[i-1]), s);
		}
		
		/// <summary>
		/// perform transformatio using i (c^(b|~d))
		/// </summary>
		protected void TransI(ref uint a, uint b, uint c, uint d,uint k,ushort s, uint i )
		{
			a = b + MD5Helper.RotateLeft((a + (c^(b|~d))+ X[k] + T[i-1]), s);
		}
		
		
		
		/// <summary>
		/// Perform All the transformation on the data
		/// </summary>
		/// <param name="A">A</param>
		/// <param name="B">B </param>
		/// <param name="C">C</param>
		/// <param name="D">D</param>
		protected void PerformTransformation(ref uint A,ref uint B,ref uint C, ref uint D)
		{
			//// saving  ABCD  to be used in end of loop
			
			uint AA,BB,CC,DD;

			AA=A;	
			BB=B;
			CC=C;
			DD=D;

			/* Round 1 
				* [ABCD  0  7  1]  [DABC  1 12  2]  [CDAB  2 17  3]  [BCDA  3 22  4]
				* [ABCD  4  7  5]  [DABC  5 12  6]  [CDAB  6 17  7]  [BCDA  7 22  8]
				* [ABCD  8  7  9]  [DABC  9 12 10]  [CDAB 10 17 11]  [BCDA 11 22 12]
				* [ABCD 12  7 13]  [DABC 13 12 14]  [CDAB 14 17 15]  [BCDA 15 22 16]
				*  * */
			TransF(ref A,B,C,D,0,7,1);TransF(ref D,A,B,C,1,12,2);TransF(ref C,D,A,B,2,17,3);TransF(ref B,C,D,A,3,22,4);
			TransF(ref A,B,C,D,4,7,5);TransF(ref D,A,B,C,5,12,6);TransF(ref C,D,A,B,6,17,7);TransF(ref B,C,D,A,7,22,8);
			TransF(ref A,B,C,D,8,7,9);TransF(ref D,A,B,C,9,12,10);TransF(ref C,D,A,B,10,17,11);TransF(ref B,C,D,A,11,22,12);
			TransF(ref A,B,C,D,12,7,13);TransF(ref D,A,B,C,13,12,14);TransF(ref C,D,A,B,14,17,15);TransF(ref B,C,D,A,15,22,16);
			/** rOUND 2
				**[ABCD  1  5 17]  [DABC  6  9 18]  [CDAB 11 14 19]  [BCDA  0 20 20]
				*[ABCD  5  5 21]  [DABC 10  9 22]  [CDAB 15 14 23]  [BCDA  4 20 24]
				*[ABCD  9  5 25]  [DABC 14  9 26]  [CDAB  3 14 27]  [BCDA  8 20 28]
				*[ABCD 13  5 29]  [DABC  2  9 30]  [CDAB  7 14 31]  [BCDA 12 20 32]
			*/
			TransG(ref A,B,C,D,1,5,17);TransG(ref D,A,B,C,6,9,18);TransG(ref C,D,A,B,11,14,19);TransG(ref B,C,D,A,0,20,20);
			TransG(ref A,B,C,D,5,5,21);TransG(ref D,A,B,C,10,9,22);TransG(ref C,D,A,B,15,14,23);TransG(ref B,C,D,A,4,20,24);
			TransG(ref A,B,C,D,9,5,25);TransG(ref D,A,B,C,14,9,26);TransG(ref C,D,A,B,3,14,27);TransG(ref B,C,D,A,8,20,28);
			TransG(ref A,B,C,D,13,5,29);TransG(ref D,A,B,C,2,9,30);TransG(ref C,D,A,B,7,14,31);TransG(ref B,C,D,A,12,20,32);
			/*  rOUND 3
				* [ABCD  5  4 33]  [DABC  8 11 34]  [CDAB 11 16 35]  [BCDA 14 23 36]
				* [ABCD  1  4 37]  [DABC  4 11 38]  [CDAB  7 16 39]  [BCDA 10 23 40]
				* [ABCD 13  4 41]  [DABC  0 11 42]  [CDAB  3 16 43]  [BCDA  6 23 44]
				* [ABCD  9  4 45]  [DABC 12 11 46]  [CDAB 15 16 47]  [BCDA  2 23 48]
			 * */
			TransH(ref A,B,C,D,5,4,33);TransH(ref D,A,B,C,8,11,34);TransH(ref C,D,A,B,11,16,35);TransH(ref B,C,D,A,14,23,36);
			TransH(ref A,B,C,D,1,4,37);TransH(ref D,A,B,C,4,11,38);TransH(ref C,D,A,B,7,16,39);TransH(ref B,C,D,A,10,23,40);
			TransH(ref A,B,C,D,13,4,41);TransH(ref D,A,B,C,0,11,42);TransH(ref C,D,A,B,3,16,43);TransH(ref B,C,D,A,6,23,44);
			TransH(ref A,B,C,D,9,4,45);TransH(ref D,A,B,C,12,11,46);TransH(ref C,D,A,B,15,16,47);TransH(ref B,C,D,A,2,23,48);
			/*ORUNF  4
				*[ABCD  0  6 49]  [DABC  7 10 50]  [CDAB 14 15 51]  [BCDA  5 21 52]
				*[ABCD 12  6 53]  [DABC  3 10 54]  [CDAB 10 15 55]  [BCDA  1 21 56]
				*[ABCD  8  6 57]  [DABC 15 10 58]  [CDAB  6 15 59]  [BCDA 13 21 60]
				*[ABCD  4  6 61]  [DABC 11 10 62]  [CDAB  2 15 63]  [BCDA  9 21 64]
						 * */
			TransI(ref A,B,C,D,0,6,49);TransI(ref D,A,B,C,7,10,50);TransI(ref C,D,A,B,14,15,51);TransI(ref B,C,D,A,5,21,52);
			TransI(ref A,B,C,D,12,6,53);TransI(ref D,A,B,C,3,10,54);TransI(ref C,D,A,B,10,15,55);TransI(ref B,C,D,A,1,21,56);
			TransI(ref A,B,C,D,8,6,57);TransI(ref D,A,B,C,15,10,58);TransI(ref C,D,A,B,6,15,59);TransI(ref B,C,D,A,13,21,60);
			TransI(ref A,B,C,D,4,6,61);TransI(ref D,A,B,C,11,10,62);TransI(ref C,D,A,B,2,15,63);TransI(ref B,C,D,A,9,21,64);


			A=A+AA;
			B=B+BB;
			C=C+CC;
			D=D+DD;


		}


		/// <summary>
		/// Create Padded buffer for processing , buffer is padded with 0 along 
		/// with the size in the end
		/// </summary>
		/// <returns>the padded buffer as byte array</returns>
		protected byte[] CreatePaddedBuffer()
		{
			uint pad;		//no of padding bits for 448 mod 512 
			byte [] bMsg;	//buffer to hold bits
			ulong sizeMsg;		//64 bit size pad
			uint sizeMsgBuff;	//buffer size in multiple of bytes
			int temp=(448-((m_byteInput.Length*8)%512)); //temporary 


			pad = (uint )((temp+512)%512);		//getting no of bits to  be pad
			if (pad==0)				///pad is in bits
				pad=512;			//at least 1 or max 512 can be added

			sizeMsgBuff= (uint) ((m_byteInput.Length)+ (pad/8)+8);
			sizeMsg=(ulong)m_byteInput.Length*8;
			bMsg=new byte[sizeMsgBuff];	///no need to pad with 0 coz new bytes 
			// are already initialize to 0 :)




			////copying string to buffer 
			for (int i =0; i<m_byteInput.Length;i++)
				bMsg[i]=m_byteInput[i];

			bMsg[m_byteInput.Length]|=0x80;		///making first bit of padding 1,
			
			//wrting the size value
			for (int i =8; i >0;i--)
				bMsg[sizeMsgBuff-i]=(byte) (sizeMsg>>((8-i)*8) & 0x00000000000000ff);

			return bMsg;
		}


		/// <summary>
		/// Copies a 512 bit block into X as 16 32 bit words
		/// </summary>
		/// <param name="bMsg"> source buffer</param>
		/// <param name="block">no of block to copy starting from 0</param>
		protected void CopyBlock(byte[] bMsg,uint block)
		{

			block=block<<6;
			for (uint j=0; j<61;j+=4)
			{
				X[j>>2]=(((uint) bMsg[block+(j+3)]) <<24 ) |
						(((uint) bMsg[block+(j+2)]) <<16 ) |
						(((uint) bMsg[block+(j+1)]) <<8 ) |
						(((uint) bMsg[block+(j)]) ) ;
			
			}
		}
	}


}
