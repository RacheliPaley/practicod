import axios from 'axios';

axios.defaults.baseURL="http://localhost:5248/todoitems";
const apiClient = axios.create({
  baseURL: "http://localhost:5248/todoitems",
});

apiClient.interceptors.response.use(
  response => response,
  error => {
    
    console.error(error);
    return Promise.reject(error);
  }
)
// const apiUrl = "http://localhost:5248"

export default {
  getTasks: async () => {
    const result = await axios.get()  
    
     
    return result.data;
  },

  addTask: async(name)=>{
    console.log('addTask', name)
    const result = await axios.post(``, {name:name,isComplete:false }) 
    return result.data;
  },

 
  setCompleted: async(id, IsComplete)=>{
    console.log('setCompleted', {id, IsComplete})
    const result = await axios.put(`/${id}`,{IsComplete:IsComplete}) 
    return result.data;
  },

  deleteTask:async(id)=>{
    const result = await axios.delete(`/${id}`) 
    return result.data
  }
};
