using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Neuron{
	

	public double netVal, outVal;
	public double[] weights, newWeights;
	public double dErrOut;
	
	public Neuron(int sizeNextLayer, int sizeCurrentLayer){
		weights = new double[sizeNextLayer];
		newWeights = new double[sizeNextLayer];
		for(int i = 0; i < sizeNextLayer; i++){
			weights[i] = (double)UnityEngine.Random.Range((float)(-1.0/Math.Sqrt(sizeCurrentLayer)), (float) (1.0/(float)Math.Sqrt(sizeCurrentLayer)) );
			newWeights[i] = weights[i];
		}
		netVal = 0; outVal = 0; dErrOut = 0;
	}
	
	public void Function(){
		outVal = 1.0 / (1+Math.Pow(Math.E,-netVal));
	}
	
	public double dFunction(){
		return outVal * (1 - outVal);
	}
}


public class NeuralNetwork{

	int[] layerSizes, accSum;
	int layers;
	public Neuron[] neurons;
	double learningRate;
	double[] avg,sd;
	public NeuralNetwork(int nLayers, int[] nLayerSizes, double nLearningRate){
		learningRate = nLearningRate;
		layers = nLayers;
		layerSizes = nLayerSizes;
		accSum = new int[layers+1];
		avg = new double[layerSizes[0]];
		sd = new double[layerSizes[0]];

		for(int i = 1; i < layers+1; i++) accSum[i] = layerSizes[i-1] + accSum[i-1];
		
		neurons = new Neuron[accSum[layers]];
		
		int k = 0;
		for(int i = 0; i < (layers-1); i++){
			for(int j = 0; j < layerSizes[i]; j++, k++){
				neurons[k] = new Neuron(layerSizes[i+1],layerSizes[i]);
			}
		}
	}
	
	
	public double[] ForwardPropagation(double[] inputs){  //Predicts
		for(int i = 0; i < layerSizes[0]; i++)
			neurons[i].outVal = inputs[i];
		
		int k = layerSizes[0];
		for(int i = 1; i < layers;i++){
			for(int j = 0; j < layerSizes[i]; j++,k++){
				neurons[k].netVal = 0;
				for(int l = 0; l < layerSizes[i-1]; l++){
					neurons[k].netVal += neurons[accSum[i-1]+l].outVal*neurons[accSum[i-1]+l].weights[j];
				}
				neurons[k].Function();
			}
		}
		
		double[] outs = new double[layerSizes[layers-1]];
		for(k = 0; k < layerSizes[layers-1]; k++){
			outs[k] = neurons[accSum[layers-1] + k].outVal;
		}

		return outs;
	}
	
	public double[] ForwardPropagationNorm(double[] inputs, double[] maxs){
		//NormalizeByTanEstimatorsOne(inputs);
		for(int i = 0; i < layerSizes[0]; i++)
			inputs[i] /= maxs[i];
		
		double[] outs = ForwardPropagation(inputs);
		//inputs[0] *= 180.0;
		//TanEstimators(outs,layerSizes[layers-1]);
		return outs;
	}
	
	public double[] GetErrors(double[] targets){
		double[] outs = new double[layerSizes[layers-1]];
		for(int i = 0; i < layerSizes[layers-1]; i++)
			outs[i] = neurons[accSum[layers-1]+i].outVal;
		
		double[] errors = new double[layerSizes[layers-1]];
		for(int i = 0; i < layerSizes[layers-1]; i++){
			errors[i] = outs[i] - targets[i];
		}
		return errors;
	}
	
	public bool Check(double[] targets, double[] errors){
		double totalError = 0;
		for(int i = 0; i < layerSizes[layers-1]; i++){
			totalError += (errors[i]*errors[i])/2.0;
		}
		
		return totalError < 0.01;
	}
	
	public void BackPropagation(double[] errors){
		
		//Output layer
		double d;//holder;
		for(int i = 0; i < layerSizes[layers-1]; i++){
			d = errors[i] * neurons[accSum[layers-1]+i].dFunction();
			//neurons[accSum[layers-1]+i].dErrOut = errors[i];
			neurons[accSum[layers-1]+i].dErrOut = d;
			for(int j = 0; j < layerSizes[layers-2]; j++){
				neurons[accSum[layers-2]+j].newWeights[i] =  neurons[accSum[layers-2]+j].weights[i] - learningRate*d*neurons[accSum[layers-2]+j].outVal;
			}
		}
		
		//Inner layer
		
		//Neuron neuron;
		for(int i = layers-2; i > 0; i--){
			for(int j = 0; j < layerSizes[i]; j++){
				d = 0;
				for(int k = 0; k < layerSizes[i+1]; k++){
					//d += (neurons[accSum[i+1]+k].dErrOut * neurons[accSum[i+1]+k].dFunction() * neurons[accSum[i]+j].weights[k]);
					d += (neurons[accSum[i+1]+k].dErrOut * neurons[accSum[i]+j].weights[k]);
				}
				//neurons[accSum[i]+j].dErrOut = d;
				d *= neurons[accSum[i]+j].dFunction();
				neurons[accSum[i]+j].dErrOut = d;
				
				
				for(int k = 0; k < layerSizes[i-1]; k++){
					neurons[accSum[i-1]+k].newWeights[j] = neurons[accSum[i-1]+k].weights[j] - learningRate*d*neurons[accSum[i-1]+k].outVal;
				}
			}
		}
		
		//Updating weights
		
		int p = 0;
		for(int i = 0; i < (layers-1); i++){
			for(int j = 0; j < layerSizes[i]; j++, p++){
				for(int l = 0; l < layerSizes[i+1]; l++){
					neurons[p].weights[l] = neurons[p].newWeights[l];
				}
			}
		}
	}
	
	void NormalizeByTanEstimatorsOne(double[] numbers){
		for(int i = 0; i < layerSizes[0]; i++){
			numbers[i] = 0.5*(Math.Tanh(0.01*((numbers[i]-avg[i])/sd[i]))+1);
		}
	}
	
	void NormalizeByTanEstimators(int cases, double[][] numbers){
		//Debug.Log(String.Format("Pre-Numbers: {0}, {1}", numbers[0], numbers[1]));
		GetAvg(cases,numbers);
		GetStandardDev(cases,numbers);
		//Debug.Log(String.Format("avg and sd: {0}, {1}", avg, sd));
		for(int i = 0; i < cases; i++){
			for(int j = 0; j < layerSizes[0]; j++){
				numbers[i][j] = 0.5*(Math.Tanh(0.01*((numbers[i][j]-avg[j])/sd[j]))+1);
			}
		}
		//Debug.Log(String.Format("Numbers: {0}, {1}", numbers[0], numbers[1]));
	}
	
	void NormalizeSimple(int cases, double[][] numbers, double[] maxs){
		for(int i = 0; i < cases; i++){
			for(int j = 0; j < layerSizes[0]; j++)
				numbers[i][j] /= maxs[j];
		}
	}
	
	
	void GetAvg(int cases, double[][] inputs){
		
		for(int i = 0; i < layerSizes[0]; i++)
			avg[i] = 0.0;
		
		for(int i = 0; i < cases; i++){
			for(int j = 0; j < layerSizes[0]; j++){
				avg[j] += inputs[i][j];
			}
		}
		
		for(int i = 0; i < layerSizes[0]; i++){
			avg[i] /= (double)cases;
			//Debug.Log(avg[i]);
		}

	}
	
	void GetStandardDev(int cases, double[][] inputs){
		
		for(int i = 0; i < layerSizes[0]; i++){
			sd[i] = 0.0;
		}
		
		for(int i = 0; i < cases; i++){
			for(int j = 0; j < layerSizes[0]; j++){
				sd[j] += (inputs[i][j] - avg[j])*(inputs[i][j] - avg[j]);
			}
		}
		
		for(int i = 0; i < layerSizes[0]; i++){
			sd[i] = Math.Sqrt(sd[i]/(double)cases);
			//Debug.Log(sd[i]);
		}

	}
	
	public void Train(int cases, double[][] inputs, double[][] targets, double[] maxs){
		//NormalizeByTanEstimators(cases,inputs);
		NormalizeSimple(cases,inputs, maxs);
		//NormalizeByTanEstimators(cases,targets);
		
		/*Debug.Log(String.Format("Target: {0}, {1}", inputs[0][0], inputs[0][1]));
		Debug.Log(String.Format("Target: {0}, {1}", inputs[1][0], inputs[1][1]));*/
		
		//return;
		
		bool stop = false;
		//int i = 10000;
		
		while(!stop){
			stop = true;
			for(int j = 0; j < cases; j++){
				//i = 10000;
				while(true){
					ForwardPropagation(inputs[j]);
					double[] errors = GetErrors(targets[j]);
					if(Check(targets[j], errors)) break;
					stop = false;
					BackPropagation(errors);
					//i--;
				}
			}
		}
	}
	
}

